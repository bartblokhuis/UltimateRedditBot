import { ElementRef } from '@angular/core';
import { Directive, Input } from '@angular/core';

@Directive({
  selector: '[CountTo]'
})
export class CountToDirective {

  @Input()
  CountTo: number = 0;
  @Input()
  from = 0;
  @Input()
  duration = 4;

  e = this.el.nativeElement;
  num: number = 0;
  refreWshInterval = 30;
  steps: number = 0;
  step = 0;
  increment: number = 0;

  constructor(private el: ElementRef) { }

  ngOnInit() {

  }

  ngOnChanges() {
    if (this.CountTo) {
      this.start();
    }
  }

  calculate() {
    this.duration = this.duration * 1000;

    this.steps = Math.ceil(this.duration / this.refreWshInterval);
    this.increment = ((this.CountTo - this.from) / this.steps);
    this.num = this.from;
  }

  tick() {
    setTimeout(() => {
      this.num += this.increment;
      this.step++;
      if (this.step >= this.steps) {
        this.num = this.CountTo;
        this.e.textContent = this.CountTo;
      } else {
        this.e.textContent = Math.round(this.num);
        this.tick();
      }
    }, this.refreWshInterval);
  }

  start() {
    this.calculate();
    this.tick();
  }

}
