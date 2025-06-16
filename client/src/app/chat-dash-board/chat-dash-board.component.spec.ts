import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatDashBoardComponent } from './chat-dash-board.component';

describe('ChatDashBoardComponent', () => {
  let component: ChatDashBoardComponent;
  let fixture: ComponentFixture<ChatDashBoardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChatDashBoardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChatDashBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
