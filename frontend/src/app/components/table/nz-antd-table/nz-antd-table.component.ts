import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTableModule, NzTableSortOrder } from 'ng-zorro-antd/table';
import { Column, NzColumn, NzEnableEditionFn } from '../../../models/column';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from "ng-zorro-antd/popconfirm";

export type TableRowActions = "edit" | "delete";

export interface SortingChangedEventArgs
{
  field?: string;
  a?:any;
  b?:any;
  sortOrder?: string
}

export interface RowActionClickedEventArgs
{
  action: TableRowActions;
  data?: any
}

@Component({
  selector: 'app-nz-antd-table',
  templateUrl: './nz-antd-table.component.html',
  styleUrl: './nz-antd-table.component.css',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzCardModule,
    NzButtonModule,
    NzTableModule,
    NzIconModule,
    NzPaginationModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    NzPopconfirmModule
  ]
})
export class NzAntdTableComponent implements OnInit
{
  @Input() page: number = 1;
  @Input() pageSize: number = 10;
  @Input() columns: Column[] = [];
  @Input() data: any[] = [];
  @Input() orderBy: string = "id";
  @Input() orderDir: string = "asc";
  @Input() count: number = 0;
  @Input() loading: boolean = false;
  @Input() readonly: boolean = false;
  @Input() canEditRowFn!: NzEnableEditionFn;
  @Input() rowActionsAllowed: TableRowActions[] = [ "delete", "edit" ];

  @Output() pageChanged: EventEmitter<number> = new EventEmitter<number>();
  @Output() pageSizeChanged: EventEmitter<number> = new EventEmitter<number>();
  @Output() sortChanged: EventEmitter<SortingChangedEventArgs> = new EventEmitter<SortingChangedEventArgs>();
  @Output() rowActionClick: EventEmitter<RowActionClickedEventArgs> = new EventEmitter<RowActionClickedEventArgs>();

  displayData: any[] = [];
  original: any[] = [];
  edited: any[] = [];
  defaultSortDirs: NzTableSortOrder[] = ['ascend', 'descend', null];

  total = 1;
  totalPages: number = 0;
  editId: number | null = null;
  isModalVisible = false;

  get nzColums() : NzColumn[] {
    const converted: NzColumn[] = this.columns.map(c => {
      const nz: NzColumn = c as NzColumn;
      nz.sortDirections = this.defaultSortDirs;

      nz.sortFn = (a:any, b:any, sortOrder: NzTableSortOrder | undefined) => {
        let so: string = 'asc';

        switch(sortOrder) {
          case 'ascend':
          default:
            so = 'asc';
            break;
          case 'descend':
            so = 'desc';
            break;
        }

        this.sortChanged.emit({
          field: nz.field,
          a,
          b,
          sortOrder: so
        });

        return -1;
      }

      return nz;
    });

    return converted;
  }

  constructor(
    private message: NzMessageService
  ) { }

  ngOnInit(): void {

  }

  onPageIndexChange(pageIndex: number): void {
    this.page = pageIndex;
    this.pageChanged.emit(pageIndex);
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageSizeChanged.emit(pageSize);
  }

  saveChanges(): void {
    this.edited = [];
    this.message.create('success', `Os dados foram salvos com sucesso!`);
  }

  cancelChanges(): void {
    this.data = this.original; // Reverte para os dados originais
    this.data = [];
  }

  handleRowActionClick(action: TableRowActions, data?: any) : void {
    this.rowActionClick.emit({ action, data });
  }
}
