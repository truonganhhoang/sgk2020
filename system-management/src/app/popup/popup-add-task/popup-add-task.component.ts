import { Component, OnInit } from '@angular/core';
import { BasePopupComponent } from 'src/app/base/base-popup/base-popup.component';

@Component({
  selector: 'app-popup-add-task',
  templateUrl: './popup-add-task.component.html',
  styleUrls: ['./popup-add-task.component.scss']
})
export class PopupAddTaskComponent extends BasePopupComponent implements OnInit {

  constructor() {
    super();
  }

  ngOnInit() {

  }

}
