import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendSendListComponent } from './friend-send-list.component';

describe('FriendSendListComponent', () => {
  let component: FriendSendListComponent;
  let fixture: ComponentFixture<FriendSendListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FriendSendListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FriendSendListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
