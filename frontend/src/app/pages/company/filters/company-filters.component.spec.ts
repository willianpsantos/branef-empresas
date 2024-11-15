import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyFiltersComponent } from './company-filters.component';

describe('CompanyFiltersComponent', () => {
  let component: CompanyFiltersComponent;
  let fixture: ComponentFixture<CompanyFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompanyFiltersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompanyFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
