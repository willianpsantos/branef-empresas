import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnChanges,
  SimpleChanges
}
from '@angular/core';

import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';

import {
  FormArray,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators
}
from '@angular/forms';

import { CommonModule } from '@angular/common';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzFlexModule } from 'ng-zorro-antd/flex';
import { ModalService } from '../../../services/modal.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { CompanyModel, CompanySize } from '../../../models/company';
import UtilsService from '../../../services/utils.service';
import { CompanyService } from '../../../services/company.service';
import { ValueLabel } from '../../../models/value-label';

export interface CompanyFormEventArgs
{
  data?: CompanyModel
}

@Component({
  selector: 'app-company-form',
  templateUrl: './company-form.component.html',
  styleUrl: './company-form.component.css',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    NzSelectModule,
    NzDatePickerModule,
    ReactiveFormsModule,
    NzFormModule,
    NzCardModule,
    NzButtonModule,
    NzInputNumberModule,
    NzFlexModule,
    NzInputModule
  ]
})
export class CompanyFormComponent implements OnInit, OnChanges
{
  @Input() data!: CompanyModel;
  @Output() afterSave: EventEmitter<CompanyFormEventArgs> = new EventEmitter<CompanyFormEventArgs>(true);
  @Output() canceled: EventEmitter<CompanyFormEventArgs> = new EventEmitter<CompanyFormEventArgs>(true);

  form!: FormGroup;
  formChildren!: FormArray;
  formChanged: boolean = false;
  saving: boolean = false;
  sizes: ValueLabel[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private modalService: ModalService,
    private messageService: NzMessageService,
    private companyService: CompanyService
  ) {
    this.sizes.push(CompanySize.SMALL);
    this.sizes.push(CompanySize.MEDIUM);
    this.sizes.push(CompanySize.LARGE);

    this.form = this.formBuilder.group({
      name: [ this.data?.Name, Validators.required ],
      size: [ this.data?.Size, Validators.required ]
    });
  }

  ngOnInit(): void {
    this.form.valueChanges.subscribe(() => {
      this.formChanged = true;
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes && changes['data'] && changes['data'].currentValue)
      this.updateForm(changes['data'].currentValue);
  }

  updateForm(data: CompanyModel) : void {
    this.form.setValue({
      'name': data?.Name ?? "",
      'size': data?.Size ?? 0
    });
  }

  updateData() : void {
    if(!this.data)
      this.data = {};

    this.data.Name = this.form.get('name')?.value;
    this.data.Size = +this.form.get('size')?.value;
  }

  onSubmit() {
    if (!this.form.valid) {
      UtilsService.markFormAsDirty(this.form);
      return;
    }

    this.updateData();
    this.saving = true;

    this.companyService.save(this.data).subscribe(response => {
      this.messageService.create('success', `O item foi cadastrado com sucesso!`);
      this.saving = false;

      this.afterSave.emit({ data: this.data });
    })
  }

  onCancel() : void {
    if (!this.formChanged) {
      this.canceled.emit({});
      return;
    }

    this.modalService.cancelar().subscribe(result => {
      if (result === 'ok') {
        this.canceled.emit({});
      }
    });
  }
}
