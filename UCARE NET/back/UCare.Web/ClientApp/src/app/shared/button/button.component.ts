import { AfterViewInit, Component, EventEmitter, HostListener, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { ThemePalette } from '@angular/material/core';

@Component({
  selector: 'gq-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss'],
})
export class ButtonComponent implements AfterViewInit {
  public isInit = false;

  @Input() public type: "mini-fab" | "fab" | "icon-button" | "flat-button" | "stroked-button" | "raised-button" | "button" | "flat-button-link" | "stroked-button-link" | "raised-button-link" | "button-form" | "button-link" = "flat-button";
  @Input() public color: ThemePalette | "secundary" = "primary";
  @Input() public icon = "";
  @Input() public label = "";
  @Input() public titleText = "";
  @Input() public classStyle = "gq-button-icon-animation";
  @Input() public positionIcon: "left" | "right" = "left";
  @Input() public classIcon = "";
  @Input() public disabled = false;

  public show: boolean = false;

  @Output() public click = new EventEmitter<ButtonComponent>();

  @ViewChild('moMiniFab')
  private moMiniFab: TemplateRef<any> | null = null;
  @ViewChild('moFab')
  private moFab: TemplateRef<any> | null = null;
  @ViewChild('moIconButton')
  private moIconButton: TemplateRef<any> | null = null;
  @ViewChild('moFlatButton')
  private moFlatButton: TemplateRef<any> | null = null;
  @ViewChild('moStrokedButton')
  private moStrokedButton: TemplateRef<any> | null = null;
  @ViewChild('moRaisedButton')
  private moRaisedButton: TemplateRef<any> | null = null;
  @ViewChild('moButton')
  private moButton: TemplateRef<any> | null = null;
  @ViewChild('moFlatButtonLink')
  private moFlatButtonLink: TemplateRef<any> | null = null;
  @ViewChild('moStrokedButtonLink')
  private MoStrokedButtonLink: TemplateRef<any> | null = null;
  @ViewChild('moRaisedButtonLink')
  private moRaisedButtonLink: TemplateRef<any> | null = null;
  @ViewChild('moButtonLink')
  private moButtonLink: TemplateRef<any> | null = null;
  @ViewChild('moButtonForm')
  private moButtonForm: TemplateRef<any> | null = null;

  public getTemplate() {
    switch (this.type) {
      case "mini-fab": return this.moMiniFab;
      case "fab": return this.moFab;
      case "icon-button": return this.moIconButton;
      case "flat-button": return this.moFlatButton;
      case "stroked-button": return this.moStrokedButton;
      case "raised-button": return this.moRaisedButton;
      case "button": return this.moButton;
      case "flat-button-link": return this.moFlatButtonLink;
      case "stroked-button-link": return this.MoStrokedButtonLink;
      case "raised-button-link": return this.moRaisedButtonLink;
      case "button-link": return this.moButtonLink;
      case "button-form": return this.moButtonForm;
    }
  }

  @HostListener('click', ['$event'])
  public onClick(event: any) {
    if (event.stopImmediatePropagation !== undefined) {
      event.stopImmediatePropagation();
      event.stopPropagation();
      (event as Event).cancelBubble = false;
      (event as Event).preventDefault();
    }

    if (this.disabled && event.stopImmediatePropagation !== undefined)
      this.click.emit(this);
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.isInit = true;
    }, 0);
  }
}
