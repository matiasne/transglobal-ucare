import { ChangeDetectorRef, Component, EventEmitter, forwardRef, Injector, Input, Output, Pipe, PipeTransform, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { QuillEditorComponent, QuillModules } from 'ngx-quill';
import Quill from 'quill';
import { RestService } from '../rest/rest.service';

@Component({
  selector: 'gq-edit-commnet',
  templateUrl: './edit-comment.component.html',
  styleUrls: ['./edit-comment.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => MOEditCommentComponent),
    multi: true
  },]
})

export class MOEditCommentComponent implements ControlValueAccessor {

  @Input() public autoSaveInSeconds: number = -1;
  @Input() public errorText: string | null = null;

  @Output() public onUploadImage: EventEmitter<any> = new EventEmitter();
  @Output() public change: EventEmitter<object> = new EventEmitter();
  @Output() public save: EventEmitter<object> = new EventEmitter();

  @ViewChild('quillEditor', { static: false, read: QuillEditorComponent }) private quillEditor: QuillEditorComponent | undefined;

  public htmlstring = "";
  public documentState = "";
  public disabled: boolean = false;
  public onChange: ((_: any) => void) | undefined = undefined;
  public onTouched: (() => void) | undefined = undefined;

  public showView: "editor" | "code" | "view" = "editor";

  public module: QuillModules = {
    toolbar: {
      container: [
        ['bold', 'italic', 'underline', 'strike'],        // toggled buttons

        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'indent': '-1' }, { 'indent': '+1' }],         // outdent/indent

        [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

        [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
        [{ 'align': [] }],
        ['link', 'image'],                                // link and image, video

        ['clean'],                                        // remove formatting button

        ['code', 'html']
      ],
      handlers: {
        'code': this.onClickCode.bind(this),
        'html': this.onClickView.bind(this)
      }
    }
  } as QuillModules;

  writeValue(obj: any): void {
    this.htmlstring = obj ?? "";
    this.showView = this.htmlstring.indexOf("<html>") > 0 ? "code" : "editor";
    this.ref.detectChanges();
  }

  registerOnChange(fn: (_: any) => void): void { this.onChange = fn; }

  registerOnTouched(fn: () => void): void { this.onTouched = fn; }

  setDisabledState?(isDisabled: boolean): void { this.disabled = isDisabled; }

  onContentChanged(event: any) {

    const value = event?.html ?? event.target.value;
    if (this.onChange !== undefined)
      this.onChange(value);
    this.change.emit(value)
  }

  public onClickCode(e: any) {
    this.showView = this.showView == "editor" ? "code" : "editor";
    this.ref.detectChanges();
  }

  public onClickView(e: any) {
    this.showView = this.showView != "view" ? "view" : "editor";
    this.ref.detectChanges();
  }

  public getHtml() {
    return `
<html>
  <head>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;900&display=swap" rel="stylesheet">
    <link href="assets/iframe.css" rel="stylesheet">
  </head>
  <body class="mat-typography" >${this.htmlstring}</body>
</html>`;
  }

  constructor(public sanitizer: DomSanitizer, injector: Injector, public ref: ChangeDetectorRef, private rest: RestService) {

    var BackgroundClass = Quill.import('attributors/class/background');
    var ColorClass = Quill.import('attributors/class/color');
    var SizeStyle = Quill.import('attributors/style/size');
    var Font = Quill.import('formats/font');
    Font.whitelist = ['roboto'];

    Quill.register(BackgroundClass, true);
    Quill.register(ColorClass, true);
    Quill.register(SizeStyle, true);
    Quill.register(Font, true);
  }
}

@Pipe({ name: 'safe' })
export class SafePipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) { }
  transform(url: string) {
    return this.sanitizer.bypassSecurityTrustHtml(url);
  }
}
