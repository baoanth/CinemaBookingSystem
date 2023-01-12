import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportOnlineListComponent } from './support-online-list.component';

describe('SupportOnlineListComponent', () => {
  let component: SupportOnlineListComponent;
  let fixture: ComponentFixture<SupportOnlineListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportOnlineListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupportOnlineListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
