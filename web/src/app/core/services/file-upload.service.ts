import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProjectExerciseSubmit } from 'src/app/domain/student/project-exercise-submit';
@Injectable({
  providedIn: 'root',
})
export class FileUploadService {
  constructor(private http: HttpClient) {}

  // Returns an observable
  upload(exercise: ProjectExerciseSubmit, baseApiUrl: string): Observable<any> {
    // Create form data
    let formData = new FormData();

    // Store form name as "file" with file data
    formData.append('projectId', exercise.id || '');
    formData.append('file', exercise.file || '', exercise.file?.name || '');

    return this.http.post(baseApiUrl, formData, {
      reportProgress: true,
      observe: 'events',
    });
  }
}
