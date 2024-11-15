import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

export enum AlertType
{
  INFORMATION = "information",
  DANGER = "danger",
  WARNING = "warning"
}

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.css',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule]
})
export class AlertComponent
{
  @Input('type') alertType: AlertType = AlertType.INFORMATION;
}
