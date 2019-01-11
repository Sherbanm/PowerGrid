import { TestBed, inject } from '@angular/core/testing';

import { CurrentPlayerService } from './current-player.service';

describe('CurrentPlayerServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CurrentPlayerService]
    });
  });

  it('should be created', inject([CurrentPlayerService], (service: CurrentPlayerService) => {
    expect(service).toBeTruthy();
  }));
});
