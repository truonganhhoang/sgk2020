import { Component, OnInit, Input, Output, EventEmitter, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { Subject, fromEvent } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DxComponent } from 'devextreme-angular';
import { KeyCode } from '../base-enum/key-code.enum';

export class BasePopupComponent implements OnInit, OnDestroy {
  public _onDestroySub: Subject<void> = new Subject<void>();
  // hiện popup
  @Input()
  visible: boolean;
  @Input()
  dataSource: any;
  @Output()
  afterClose = new EventEmitter<any>();
  @Output()
  afterSave = new EventEmitter<any>();
  @Output()
  afterCancel = new EventEmitter<any>();
  @ViewChild("focusItem", { static: false })
  focusItem: ElementRef;

  @ViewChild("enterTarget", { static: false })
  enterTarget: ElementRef;
  //
  constructor() { }
  ngOnDestroy(): void {
      this._onDestroySub.next();
      this._onDestroySub.complete();
      this._onDestroySub.unsubscribe();
  }

  ngOnInit(): void { }
  // hàm  sau khi đóng popup
  closePopup() {
      this.visible = false;
      this.afterClose.emit();
  }

  onShown(e) {
      const me = this;
      if (me.focusItem instanceof DxComponent) {
          me.focusItem["instance"].focus();
      } else {

      }
      // Bắt sự kiện keyup
      fromEvent(window, "keyup").pipe(takeUntil(this._onDestroySub)).subscribe((e) => {
          const keyCode = event["keyCode"];
          if (Object.values(KeyCode).includes(keyCode)) {
              switch (keyCode) {
                  case KeyCode.Esc:
                      me.visible = false;
                      break;
                  case KeyCode.Enter:
                      if (me.enterTarget) {
                          if (e.target["nodeName"] === "TEXTAREA") {
                              return;
                          }
                          if (me.enterTarget instanceof DxComponent) {
                              me.enterTarget["element"].nativeElement.click();
                          } else {
                              me.enterTarget.nativeElement.click();

                          }
                      }
                      break;
                  default:
                      break;
              }
          }
      });
  }

}
