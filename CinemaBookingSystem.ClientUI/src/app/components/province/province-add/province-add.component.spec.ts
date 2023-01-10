import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvinceAddComponent } from './province-add.component';

describe('ProvinceAddComponent', () => {
  let component: ProvinceAddComponent;
  let fixture: ComponentFixture<ProvinceAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProvinceAddComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProvinceAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
