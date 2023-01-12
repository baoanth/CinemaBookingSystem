import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportOnlineAddComponent } from './support-online-add.component';

describe('SupportOnlineAddComponent', () => {
  let component: SupportOnlineAddComponent;
  let fixture: ComponentFixture<SupportOnlineAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportOnlineAddComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupportOnlineAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
