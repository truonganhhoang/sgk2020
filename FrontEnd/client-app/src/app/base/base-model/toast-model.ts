export class Toast {
    text: string;
    type: string;
    displayTime: number;
    constructor(t = "Thao tác thành công!", p = "success", d = 600) {
        this.text = t;
        this.type = p;
        this.displayTime = d;
    }
}