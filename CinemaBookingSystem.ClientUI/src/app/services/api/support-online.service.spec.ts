import { TestBed } from '@angular/core/testing';

import { SupportOnlineService } from './support-online.service';

describe('SupportOnlineService', () => {
  let service: SupportOnlineService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SupportOnlineService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
