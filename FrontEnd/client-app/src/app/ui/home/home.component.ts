import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import notify from "devextreme/ui/notify";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  visiblePopupAddTask: boolean;
    /**
   * Danh sách màu từng cột của chart
   */
  colorSeries = ["#d1d8e0", "#45aaf2", "#a55eea", "#fd9643", "#26de81"];
  constructor(private el: ElementRef) { }

  ngOnInit() {
    this.visiblePopupAddTask = false;
    this.initFunnel();
  }
  show() {
    // notify("Chức năng đang thi công!", "success", 600);
    this.visiblePopupAddTask = true;
  }

  // ----------- biểu đồ ------------
  initFunnel() {

  }
  fnRenderTooltip(data) {
    return `<span>hi!</span>`;
  }
}
