import { Component, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EncodingType } from './encoding-type';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {
  protected readonly title = signal('3dhistechhomework.client');

  file: File | null = null;
  resultImage: string | null = null;

  encoding: EncodingType = EncodingType.Png;
  EncodingType = EncodingType;

  constructor(private http: HttpClient) { }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      this.file = input.files[0];
    }
  }

  upload() {
    if (!this.file) {
      alert('Please select a file first');
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      const base64 = (reader.result as string).split(',')[1];

      this.http.post('https://localhost:7250/api/image/process', {
        imageBase64: base64,
        outputEncoding: this.encoding
      }, { responseType: 'blob' })
        .subscribe(blob => {
          if (this.resultImage) {
            URL.revokeObjectURL(this.resultImage);
          }

          this.resultImage = URL.createObjectURL(blob);
        }, error => {
          console.error('Image processing failed', error);
        });
    };

    reader.readAsDataURL(this.file);
  }

  download() {
    if (!this.resultImage) return;

    const a = document.createElement('a');
    a.href = this.resultImage;
    a.download = this.encoding === EncodingType.Png
      ? 'processed.png'
      : 'processed.jpg';

    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
  }
}
