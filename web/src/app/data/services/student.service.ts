import { HttpClient, HttpEvent } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FileUploadService } from 'src/app/core/services/file-upload.service';
import {
  ExerciseResponse,
  ExerciseSubmit,
  StudentCourse,
  StudentExercise,
  StudentRating,
  Topic,
  TopicList,
} from 'src/app/domain';
import { ProjectExerciseSubmit } from 'src/app/domain/student/project-exercise-submit';
import { StudentProjectExercise } from 'src/app/domain/student/student-project-exercise';
import { StudentTest } from 'src/app/domain/student/student-test';
import { TestResponse } from 'src/app/domain/student/test-response';
import { TestSubmit } from 'src/app/domain/student/test-submit';
import { BASE_API_URL } from 'src/app/injectors';
import { BaseService } from '../base/base-service';
import { DataModulte } from '../data.module';

@Injectable({
  providedIn: DataModulte,
})
export class StudentService extends BaseService {
  protected segment: string = 'student';
  constructor(_http: HttpClient, @Inject(BASE_API_URL) apiUrl: string, public fileUpload: FileUploadService) {
    super(apiUrl, _http);
  }

  getRates(courseId: string): Observable<StudentRating> {
    return this._http.get<StudentRating>(this.getPath() + 'rating/' + courseId);
  }

  getCourses(): Observable<StudentCourse[]> {
    return this._http.get<StudentCourse[]>(this.getPath() + 'courses');
  }

  getTopicList(courseId: string): Observable<TopicList[]> {
    return this._http.get<TopicList[]>(this.getPath() + 'themes/' + courseId);
  }

  getTopicById(id: string): Observable<Topic> {
    return this._http.get<Topic>(this.getPath() + 'theme/' + id);
  }

  getExercises(topicId: string): Observable<StudentExercise[]> {
    return this._http.get<StudentExercise[]>(this.getPath() + `exercises/${topicId}`);
  }

  checkExercise(exercise: ExerciseSubmit): Observable<ExerciseResponse> {
    return this._http.post<ExerciseResponse>(this.getPath() + 'checkExercise', exercise);
  }

  getTests(topicId: string): Observable<StudentTest[]> {
    return this._http.get<StudentTest[]>(this.getPath() + `tests/${topicId}`);
  }

  checkTest(exercise: TestSubmit): Observable<TestResponse> {
    return this._http.post<TestResponse>(this.getPath() + 'checkTest', exercise);
  }

  getProjectExercises(topicId: string): Observable<StudentProjectExercise[]> {
    return this._http.get<StudentProjectExercise[]>(this.getPath() + `ProjectExercises/${topicId}`);
  }

  submitProjectExercise(exercise: ProjectExerciseSubmit): Observable<HttpEvent<ExerciseResponse>> {
    return this.fileUpload.upload(exercise, this.getPath() + 'SubmitProjectExercise');
  }
}
