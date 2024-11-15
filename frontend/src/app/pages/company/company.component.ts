import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, BehaviorSubject, switchMap } from 'rxjs';
import { CompanyModel } from "../../models/company";

import { PageResponse } from '../../models/page-response';
import { CompanyService } from '../../services/company.service';
import { ListOptions } from '../../models/list.options';
import { QueryFilter } from '../../models/filter';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzAntdTableComponent, RowActionClickedEventArgs, SortingChangedEventArgs } from '../../components/table/nz-antd-table/nz-antd-table.component';
import { Column } from '../../models/column';
import { CompanyFiltersComponent } from "./filters/company-filters.component";
import { CompanyFormComponent, CompanyFormEventArgs } from './form/company-form.component';

import { NzDrawerModule, NzDrawerService, NzDrawerRef } from 'ng-zorro-antd/drawer';
import { ValueLabel } from '../../models/value-label';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrl: './company.component.css',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzAntdTableComponent,
    CompanyFiltersComponent,
    CompanyFormComponent,
    NzDrawerModule
  ]
})
export class CompanyComponent implements OnInit
{
  columns: Column[] = [
    {
      title: "Nome da empresa",
      field: "name",
      width: "45%",
      sortable: true,
      filter: true,
      dataType: "string"
    },

    {
      title: "Tamanho da empresa",
      field: "sizeName",
      width: "15%",
      sortable: true,
      filter: true,
      dataType: "string"
    }
  ];

  currentListResponse: PageResponse<CompanyModel> = {
    data: [] as CompanyModel[],
    count: 0,
    page: 1,
    take: 10
  } as PageResponse<CompanyModel>;

  currentFilterVersion: number = 0;
  currentFilters: QueryFilter[] = [];
  currentPage: number = 1;
  currentPageSize: number = 10;
  currentEditing!: CompanyModel;

  filterVersionBehaviorSubject: BehaviorSubject<number> = new BehaviorSubject<number>(this.currentFilterVersion);
  dataObservable: Observable<PageResponse<CompanyModel>> = new Observable<PageResponse<CompanyModel>>();
  data: CompanyModel[] = [];
  isFormOpened: boolean = false;
  isTableLoading: boolean = false;

  get totalPages(): number {
    return Math.ceil((this.currentListResponse?.count ?? 0) / this.currentPageSize);
  }

  constructor(
    public drawerService: NzDrawerService,
    private companyService: CompanyService
  ) {

  }

  loadData(): void {
    this.isTableLoading = true;
    const version: number = this.currentFilterVersion + 1;
    this.filterVersionBehaviorSubject.next(version);
  }

  ngOnInit(): void {
    this.dataObservable = this
      .filterVersionBehaviorSubject
      .pipe(
        switchMap(
          (version: number) => {
            if(version == this.currentFilterVersion)
              return new Observable<PageResponse<CompanyModel>>(observer => observer.next(this.currentListResponse));

            this.currentFilterVersion = version;

            const options = new ListOptions();

            options.page = this.currentPage,
            options.take = this.currentPageSize,
            options.filters = this.currentFilters;

            return this.companyService.listAll(options);
          }
        )
      );

    this.dataObservable.subscribe(response => {
      this.currentListResponse = response;
      this.data = response?.data ?? [];
      this.isTableLoading = false;
    });

    this.loadData();
  }

  removeScrollBlockClass() : void {
    document.scrollingElement?.classList.remove('cdk-global-scrollblock');
  }

  openForm(editing: boolean): void {
    if (!editing)
      this.currentEditing = {};

    this.isFormOpened = true;
  }

  drawerClosed(e: MouseEvent) : void {
    this.isFormOpened = false;
    this.removeScrollBlockClass();
  }

  drawerFormCanceled(e: CompanyFormEventArgs) : void {
    this.isFormOpened = false;
    this.removeScrollBlockClass();
  }

  drawerFormAfterSaved(e: CompanyFormEventArgs) : void {
    this.isFormOpened = false;
    this.removeScrollBlockClass();
    this.loadData();
  }

  tableFilterChanged(args: QueryFilter[]): void {
    this.currentPage = 1;
    this.currentFilters = args;
    this.loadData();
  }

  tablePageSizeChanged(args: number): void {
    if (this.currentPageSize == args)
      return;

    this.currentPageSize = args;
    this.loadData();
  }

  tablePageChanged(args: number): void {
    if (this.currentPage == args)
      return;

    this.currentPage = args;
    this.loadData();
  }

  tableRowActionClick(args: RowActionClickedEventArgs) : void {
    switch(args.action) {
      case 'delete':
        this.isTableLoading = true;

        this.companyService.delete(args.data.id).subscribe(response => {
          this.isTableLoading = false;
          this.loadData();
        });
        break;
      case 'edit':
        const { id, name, size} = args.data;
        this.currentEditing = { Id: id, Name: name, Size: size };
        this.openForm(true);
        break;
    }
  }
}
