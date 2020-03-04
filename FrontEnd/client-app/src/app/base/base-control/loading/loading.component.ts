import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'spiner-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent implements OnInit {


  @Input() fullScreen: boolean = false

  @Input() bdColor: string

  @Input() color: string

  @Input() text: string = "Đang tải . . ."
  constructor() { }

  ngOnInit() {
  }

}
