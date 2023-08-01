import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Documentation } from 'src/app/domain/documentation';
import { Teacher } from 'src/app/domain/teacher';
import { BASE_API_URL } from 'src/app/injectors';
import { BaseService } from '../base/base-service';
import { DataModulte } from '../data.module';

@Injectable({
  providedIn: DataModulte,
})
export class AdminService extends BaseService {
  protected segment: string = 'admin';
  constructor(_http: HttpClient, @Inject(BASE_API_URL) apiUrl: string) {
    super(apiUrl, _http);
  }

  getTeachers(): Observable<Teacher[]> {
    return this._http.get<Teacher[]>(this.getPath() + 'teachers');
  }

  createTeacher(item: Teacher): Observable<string> {
    return this._http.post<string>(this.getPath() + 'createteacher', item);
  }

  editTeacher(item: Teacher): Observable<any> {
    return this._http.put<Teacher>(this.getPath() + 'updateteacher', item);
  }

  deleteTeacher(teacherId: string): Observable<any> {
    return this._http.delete(this.getPath() + 'deleteteacher/' + teacherId);
  }

  getDocs(): Observable<Documentation[]> {
    return this._http.get<Documentation[]>(this.getPath() + 'docs');
  }

  createDoc(item: Documentation): Observable<string> {
    return this._http.post<string>(this.getPath() + 'createdoc', item);
  }

  editDoc(item: Documentation): Observable<any> {
    return this._http.put<Documentation>(this.getPath() + 'updatedoc', item);
  }

  deleteDoc(docId: string): Observable<any> {
    return this._http.delete(this.getPath() + 'deletedoc/' + docId);
  }
}
