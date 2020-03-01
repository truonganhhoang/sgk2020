import { Component, OnInit, AfterViewInit, Input, NgZone } from "@angular/core";

declare const $: any;
declare const Highcharts: any;
@Component({
  selector: "vig-funnel-chart",
  templateUrl: "./vig-funnel-chart.component.html",
  styleUrls: ["./vig-funnel-chart.component.scss"],
  host: {
    class: "wrap-vig-funnel-chart"
  }
})
export class VigFunnelChartComponent implements OnInit, AfterViewInit {
  static nextId: number = 0;

  @Input()
  id: string = "";

  @Input()
  width: string = "";

  @Input()
  height: string = "100%";

  /**
   * Tiêu đề biểu đồ
   */
  @Input()
  title: string = "";

  /**
   * Sub tiêu đề biểu đồ
   */
  @Input()
  subtitle: string = "";

  /**
   * Tiêu đề trục y
   */
  @Input()
  yAxisTitle: string = "";

  /**
   * Tên trường hiển thị ở trục x
   */
  @Input()
  xField: string = "Text";

  /**
   * Danh sách column hiển thị ở trên trục x
   */
  @Input()
  yField: string = "Amount";

  /**
   * Ghi chú các cột trên trục x
   */
  @Input()
  titleSeries: string[];

  /**
   * Ghi chú các cột trên trục x
   */
  @Input()
  fnRenderFormat: Function = null;

  /**
   * render lai cot yField
   */
  @Input()
  fnRenderYField: Function = null;

  /**
   * render lại tooltip
   */
  @Input()
  fnRenderTooltip: Function = null;

  /**
   * Danh sách màu cho từng cột
   */
  @Input()
  colorSeries: [];

  /**
   * Data của biểu đồ
   */
  @Input()
  dataSource: any = [
    { Text: "Ứng tuyển", Amount: 421 },
    { Text: "Thi tuyển", Amount: 20 },
    { Text: "Phỏng vấn", Amount: 20 },
    { Text: "Offer", Amount: 1 },
    { Text: "Đã tuyển", Amount: 1 }
  ];

  private seriesData = [];

  myChart: any;

  constructor(public zone: NgZone) {
    this.id = `crm-funnel-chart-${VigFunnelChartComponent.nextId++}`;
  }

  /**
   * Thực hiện xử lý data trước khi hiển thị lên biểu đồ
   */
  private processData() {
    this.seriesData = [];
    if (this.dataSource) {
      this.dataSource.forEach(element => {
        const vVal = element[this.yField];
        if (this.fnRenderYField) {
          this.seriesData.push([
            element[this.xField],
            this.fnRenderYField(vVal)
          ]);
        } else {
          this.seriesData.push([element[this.xField], vVal]);
        }
      });
    }
  }

  /**
   * Gán lại data cho chart
   */
  setData(data: any) {
    this.dataSource = data;
    this.processData();
    this.zone.runOutsideAngular(() => {
      const series = this.myChart.series;
      if (series.length > 0) {
        series[0].setData(this.seriesData, false);
        this.myChart.redraw();
      }
    });
  }

  ngOnInit() {}

  ngAfterViewInit(): void {
    const me = this;
    this.processData();

    const callBackFormat = () => {
      if (me.fnRenderFormat) {
        return me.fnRenderFormat();
      }
      return `<span style="font-weight: 500">{point.name}: {point.y:,.0f}</span>`;
    };

    const callBackTooltip = data => {
      if (me.fnRenderTooltip) {
        return me.fnRenderTooltip(data);
      }
      return `<span>${data.key}</span>: <b>${
        data.y
      }</b><br/>`;
    };

    this.zone.runOutsideAngular(() => {
      Highcharts.setOptions({
        lang: {
          thousandsSep: "."
        }
      });
      this.myChart = Highcharts.chart(this.id, {
        chart: {
          type: "funnel"
        },
        title: {
          text: this.title
        },
        plotOptions: {
          series: {
            dataLabels: {
              enabled: true,
              format: callBackFormat(),
              color:
                (Highcharts.theme && Highcharts.theme.contrastTextColor) ||
                "black",
              softConnector: true
            },
            center: ["40%", "50%"],
            neckWidth: "30%",
            neckHeight: "25%",
            width: "75%"
          }
        },
        legend: {
          enabled: false
        },
        series: [
          {
            data: this.seriesData
          }
        ],
        tooltip: {
          formatter: function() {
            return callBackTooltip(this);
          }
        },
        colors: this.colorSeries
      });
    });
  }
}
