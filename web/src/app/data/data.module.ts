import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { environment } from 'src/environments/environment';
import { FileUploadService } from '../core/services/file-upload.service';
import { BASE_API_URL } from '../injectors';

@NgModule({
  imports: [HttpClientModule],
  providers: [{ provide: BASE_API_URL, useValue: environment.baseUrl }, FileUploadService],
})
export class DataModulte {}
