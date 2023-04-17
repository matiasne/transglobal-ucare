import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from "@angular/core";

@Component({
  selector: 'gq-image',
  templateUrl: './image.component.html',
  styleUrls: ['./image.component.scss'],
})
export class ImageComponent {

  @ViewChild('contentImage', { static: false }) private _contentImage: ElementRef<HTMLDivElement> | undefined;

  public imageSrc: string | ArrayBuffer | null = null;
  public showImage: boolean = true;

  @Input()
  public accept: string = "image/*"

  private _image: string = "";

  @Input()
  public get image(): string {
    return this._image;
  }
  public set image(value: string) {
    this._image = value;
    this.imageSrc = `${value}?r=${Math.random()}`;
  }

  @Output()
  public onFileChange: EventEmitter<any> = new EventEmitter();

  public onUploadImage(event: any) {
    const fileTypes = ['jpg', 'jpeg', 'png', 'gif', 'svg'];

    if (this._contentImage !== undefined)
      this._contentImage.nativeElement.innerHTML = "";
    this.showImage = true;

    if (event !== undefined && event !== null) {
      if (event.target.files.length == 1) {
        this.onFileChange.emit(event.target.files[0]);

        var file = event.target.files[0];
        const extension = file.name.split('.').pop().toLowerCase(),
          isSuccess = fileTypes.indexOf(extension) > -1;

        if (isSuccess) {
          var fr = new FileReader();
          if (extension == "svg") {
            fr.onload = (loadImage) => {
              if (this._contentImage !== undefined && fr.result !== null)
                this._contentImage.nativeElement.innerHTML = fr.result.toString();
              this.showImage = false;
            }   // onload fires after reading is complete
            fr.readAsText(file);
          }
          else {
            fr.onload = (loadImage) => {
              this.imageSrc = fr.result;
              this.showImage = true;
            }   // onload fires after reading is complete
            fr.readAsDataURL(file);
          }
        }
      }
    }
  }
}
