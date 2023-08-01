import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Documentation } from 'src/app/domain/documentation';
import { UseCase } from '../../../base/use-case';
import { DataModulte } from '../../../data.module';
import { AdminService } from '../../../services/admin.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetDocumentationsUseCase implements UseCase<void, Documentation[]> {
  constructor(private adminService: AdminService) {}

  execute(): Observable<Documentation[]> {
    return this.adminService.getDocs().pipe(map(x => x.map(t => Object.assign(new Documentation(), t))));
  }
}
