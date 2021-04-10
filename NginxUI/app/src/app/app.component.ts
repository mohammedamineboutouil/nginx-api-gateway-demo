import { Component, OnDestroy } from '@angular/core';
import { worker } from 'node:cluster';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnDestroy {
  title = 'app';
  worker: any;
  data: any = [];

  /**
   *
   */
  constructor() {
    if (typeof Worker !== 'undefined') {
      // Create a new
      const ww = new Worker('./app.worker', { type: 'module' });
      ww.onmessage = ({ data }) => {
        console.info('Worker Done');
        this.data = JSON.parse(data);
      };

      this.worker = ww;
    } else {
      // Web Workers are not supported in this environment.
      // You should add a fallback so that your program still executes correctly.
    }
  }

  ngOnDestroy(): void {
    (this.worker as Worker).terminate();
  }

  onClick() {
    this.worker.postMessage(300);
  }
}
