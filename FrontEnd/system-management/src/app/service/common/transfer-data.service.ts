import { Injectable, Output, EventEmitter } from '@angular/core';
import { Toast } from 'src/app/base/base-model/toast-model';

@Injectable({
  providedIn: 'root'
})
/// Output = nơi gọi subscribe hứng dữ liệu
/// hàm tương ứng output = nhận đầu vào dữ liệu
export class TransferDataService {
// thông báo
  @Output()
  notification: EventEmitter<any> = new EventEmitter();
  constructor() { }
  // thông báo
  valueChangedNotify(data: Toast) {
    this.notification.emit(data);
  }
}
