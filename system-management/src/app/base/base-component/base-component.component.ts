import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

export class BaseComponent implements OnInit, OnDestroy {
  // tslint:disable-next-line:variable-name
  public unSub: Subject<void> = new Subject<void>();

  constructor() { }

  ngOnInit() {
  }
  ngOnDestroy(): void {
    this.unSub.next();
    this.unSub.complete();
    this.unSub.unsubscribe();
  }
}
