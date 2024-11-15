import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Component, Input, OnInit, ViewChild, AfterViewInit, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ColumnDataType, FilterChangedArgs, FilterValue, QueryFilter } from '../../../models/filter';
import UtilsService from '../../../services/utils.service';
import { PageResponse } from '../../../models/page-response';
import { Column } from '../../../models/column';

@Component({
  selector: 'app-ng-mat-table',
  templateUrl: './ng-mat-table.component.html',
  styleUrls: ['./ng-mat-table.component.scss'],
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatTooltipModule,
    MatButtonModule,
    MatMenuModule,
    MatIconModule
  ]
})
export class AngularMaterialTableComponent implements OnInit, AfterViewInit, OnChanges
{
  @Input() columns: Column[] = [];
  @Input() data: any[] = [];
  @Input() count: number = 0;
  @Input() page: number = 1;
  @Input() pageSize: number = 10;

  @Output() filterChanged: EventEmitter<FilterChangedArgs> = new EventEmitter<FilterChangedArgs>(true);

  displayedColumns: string[] = [];
  dataSource = new MatTableDataSource<any>();
  perPageOptions: number[] = [10, 12];
  totalPages: number = 0;
  filters: { [key: string]: FilterValue } = {};

  @ViewChild(MatSort) sort: MatSort = new MatSort();

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource<any>(this.data);
    this.displayedColumns = this.columns.map(column => column.field).concat('actions');
    this.totalPages = Math.ceil(this.count / this.pageSize);
    this.page = 1;

    this.columns.forEach(column => {
      if (column.filter) {
        this.filters[column.field] = { value: '', type: column.dataType ?? 'string' };
      }
    });

    this.applyFilters();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  ngOnChanges(changes: SimpleChanges): void {
    changes['data'] && (this.dataSource.data = changes['data'].currentValue as any[]);
    this.totalPages = Math.ceil(this.count / this.pageSize);
  }

  truncateText(text: string, maxLength: number): string {
    return UtilsService.truncateText(text, maxLength);
  }


  handlePrevClick() {
    if (this.page > 1) {
      this.page--;
      this.applyFilters();
    }
  }

  handleNextClick() {
    if (this.page < this.totalPages) {
      this.page++;
      this.applyFilters();
    }
  }

  handlePerPageChange() {
    this.page = 1;
    this.totalPages = Math.ceil(this.count / this.pageSize);
    this.applyFilters();
  }

  setCurrentPage(page: number) {
    this.page = page;
    this.applyFilters();
  }

  getPageNumbers(): number[] {
    const pageNumbers = [];
    const maxPagesToShow = 4;
    const startPage = Math.max(1, this.page - Math.floor(maxPagesToShow / 2));
    const endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);

    for (let i = startPage; i <= endPage; i++) {
      pageNumbers.push(i);
    }

    return pageNumbers;
  }

  applyFilters() {
    const filters = QueryFilter.toQueryFilters(this.filters);

    const args: FilterChangedArgs = {
      columns: this.columns,
      count: this.count,
      page: this.page,
      take: this.pageSize,
      data: this.data,
      filters
    } as FilterChangedArgs;

    this.filterChanged.emit(args);
  }

  deleteRow(row: any): void {
    const index = this.data.indexOf(row);
    if (index >= 0) {
      this.data.splice(index, 1);
      this.applyFilters();
    }
  }
}
