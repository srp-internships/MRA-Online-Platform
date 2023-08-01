import { UseCase } from 'src/app/data/base/use-case';
import { Documentation } from 'src/app/domain/documentation';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataModulte } from 'src/app/data/data.module';
import { AdminService } from 'src/app/data/services/admin.service';

@Injectable({
  providedIn: DataModulte,
})
export class CreateDocumentationUseCase implements UseCase<Documentation, string> {
  constructor(private adminService: AdminService) {}

  execute(item: Documentation): Observable<string> {
    return this.adminService.createDoc(item);
  }
}
