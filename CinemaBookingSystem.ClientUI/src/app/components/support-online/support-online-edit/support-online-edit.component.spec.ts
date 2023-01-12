import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportOnlineEditComponent } from './support-online-edit.component';

describe('SupportOnlineEditComponent', () => {
  let component: SupportOnlineEditComponent;
  let fixture: ComponentFixture<SupportOnlineEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportOnlineEditComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupportOnlineEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
