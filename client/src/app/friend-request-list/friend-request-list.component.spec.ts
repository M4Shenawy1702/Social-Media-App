import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendRequestListComponent } from './friend-request-list.component';

describe('FriendRequestListComponent', () => {
  let component: FriendRequestListComponent;
  let fixture: ComponentFixture<FriendRequestListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FriendRequestListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FriendRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
