import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzDropDownModule, NzDropdownMenuComponent } from 'ng-zorro-antd/dropdown';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTableModule } from 'ng-zorro-antd/table';

@Component({
    selector: 'app-delete-modal',
    templateUrl: './delete-modal.component.html',
    styleUrls: ['./delete-modal.component.css'],
    standalone : true,

    imports: [
      CommonModule,
      FormsModule,
      ReactiveFormsModule,
      NzFormModule,
      NzCardModule,
      NzButtonModule,
      NzSelectModule,
      NzDatePickerModule,
      NzTableModule,
      NzIconModule,
      NzDropDownModule,
      NzPaginationModule,
      NzDropdownMenuComponent,
      NzInputModule,
      NzModalModule
    ]
})
export class DeleteModalComponent {
    @Input() isVisible = false;

    @Output() onOk = new EventEmitter<void>();
    @Output() onCancel = new EventEmitter<void>();

    handleOk(): void {
        this.onOk.emit();
        this.isVisible = false;
    }

    handleCancel(): void {
        this.onCancel.emit();
        this.isVisible = false;
    }
}
