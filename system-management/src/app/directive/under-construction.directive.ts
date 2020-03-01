import { Directive, HostListener, ElementRef, NgZone } from "@angular/core";
import notify from "devextreme/ui/notify";
import { TransferDataService } from '../service/common/transfer-data.service';
@Directive({
  selector: "[appUnderConstruction]"
})
export class UnderConstructionDirective {

  //#region property Custom
  @HostListener("click", ["$event"])
  onClick($event: UIEvent) {
    this.click($event);
  }
  //#endregion

  //#region Overrides property
  //#endregion

  //#region LifeCycle
  // Hàm dựng
  constructor(private tfsv: TransferDataService, private element: ElementRef, private zone: NgZone) { }
  //#endregion

  //#region  Fuction
  // Xử lý sự kiện click
  click(e): void {
    const me = this;
    me.zone.runOutsideAngular(() => {
      notify("Chức năng đang thi công!", "success", 600);
    });
  }
  //#endregion

}
