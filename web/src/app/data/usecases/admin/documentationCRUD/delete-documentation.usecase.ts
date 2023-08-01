import { UseCase } from 'src/app/data/base/use-case';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataModulte } from 'src/app/data/data.module';
import { AdminService } from 'src/app/data/services/admin.service';

@Injectable({
  providedIn: DataModulte,
})
export class DeleteDocumentationUseCase implements UseCase<string, any> {
  constructor(private adminService: AdminService) {}

  execute(id: string): Observable<any> {
    return this.adminService.deleteDoc(id);
  }
}
