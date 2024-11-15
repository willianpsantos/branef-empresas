import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndiceTableComponent } from './nz-antd-table.component';

describe('IndiceTableComponent', () => {
  let component: IndiceTableComponent;
  let fixture: ComponentFixture<IndiceTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IndiceTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IndiceTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
