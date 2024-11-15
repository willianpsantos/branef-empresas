import { QueryFilter } from './../../../models/filter';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { ValueLabel } from '../../../models/value-label';
import { CompanySize } from '../../../models/company';

@Component({
  selector: 'app-company-filter',
  templateUrl: './company-filters.component.html',
  styleUrl: './company-filters.component.css',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzCardModule,
    NzButtonModule,
    NzSelectModule,
    NzDatePickerModule,
    NzIconModule,
    NzInputModule
  ],

  encapsulation: ViewEncapsulation.None
})
export class CompanyFiltersComponent implements OnInit
{
  @Output() filterChanged: EventEmitter<QueryFilter[]> = new EventEmitter<QueryFilter[]>();

  form!: FormGroup;
  isCollapsed = false;
  sizes: ValueLabel[] = [];

  constructor(
    private fb: FormBuilder
  ) {
    this.sizes.push(CompanySize.SMALL);
    this.sizes.push(CompanySize.MEDIUM);
    this.sizes.push(CompanySize.LARGE);
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: new FormControl(),
      size: new FormControl()
    });

    this.form.valueChanges.subscribe(value => {
      this.submitFilters();
    })
  }

  toggleCollapse() {
    this.isCollapsed = !this.isCollapsed; // Alterna o estado de visibilidade
  }

  clearFilters() {
    this.form.reset();
    this.submitFilters();
  }

  submitFilters() {
    const name: string = this.form.controls['name'].value;
    const size: number = this.form.controls['size'].value;
    const filters: QueryFilter[] = [];

    if (name !== undefined && name !== null)
      filters.push(new QueryFilter({ field:'name', value: name }));

    if (size !== undefined && size !== null)
      filters.push(new QueryFilter({ field:'size', value: size }));

    this.filterChanged.emit(filters);
  }
}
