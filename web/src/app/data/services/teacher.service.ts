import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Course, Excercise, StudentsRating, StudentsRatingModel, Test, Topic } from 'src/app/domain';
import { ProjectExercise } from 'src/app/domain/project-exercise';
import { Variant } from 'src/app/domain/variant';
import { BASE_API_URL } from 'src/app/injectors';
import { BaseService } from '../base/base-service';
import { DataModulte } from '../data.module';
import { StudentsRatingMapper } from '../mappers/students-rating.mapper';

@Injectable({
  providedIn: DataModulte,
})
export class TeacherService extends BaseService {
  protected segment: string = 'teacher';
  constructor(_http: HttpClient, @Inject(BASE_API_URL) apiUrl: string) {
    super(apiUrl, _http);
  }

  getStudnetsRating(courseId: string): Observable<StudentsRating[]> {
    return this._http
      .get<StudentsRatingModel[]>(this.getPath() + 'rating/' + courseId)
      .pipe(map(s => s.map(x => new StudentsRatingMapper().mapTo(x))));
  }

  getCourses(): Observable<Course[]> {
    return this._http.get<Course[]>(this.getPath() + 'courses');
  }

  createCourse(item: Course): Observable<string> {
    return this._http.post<string>(this.getPath() + 'createcourse', item);
  }

  editCourse(item: Course): Observable<any> {
    return this._http.put<Course>(this.getPath() + 'updatecourse', item);
  }

  deleteCourse(id: string): Observable<any> {
    return this._http.delete(this.getPath() + 'deletecourse/' + id);
  }

  getTopics(courseId: string): Observable<Topic[]> {
    return this._http.get<Topic[]>(this.getPath() + 'themes/' + courseId);
  }

  createTopic(topic: Topic): Observable<string> {
    return this._http.post<string>(this.getPath() + 'createtheme', topic);
  }

  editTopic(topic: Topic): Observable<any> {
    return this._http.put<any>(this.getPath() + 'updatetheme', topic);
  }

  deleteTopic(id: string): Observable<any> {
    return this._http.delete<any>(this.getPath() + 'deletetheme/' + id);
  }

  getExercises(topicId: string): Observable<Excercise[]> {
    return this._http.get<Excercise[]>(this.getPath() + 'exercises/' + topicId);
  }

  craeteExercise(item: Excercise): Observable<string> {
    return this._http.post<string>(this.getPath() + 'createexercise', item);
  }

  editExercise(item: Excercise): Observable<any> {
    return this._http.put<any>(this.getPath() + 'updateexercise', item);
  }

  deleteExercise(id: string): Observable<any> {
    return this._http.delete<any>(this.getPath() + 'deleteexercise/' + id);
  }

  getTests(topicId: string): Observable<Test[]> {
    return this._http.get<Test[]>(this.getPath() + 'tests/' + topicId);
  }

  craeteTest(item: Test): Observable<string> {
    return this._http.post<string>(this.getPath() + 'createtest', item);
  }

  editTest(item: Test): Observable<any> {
    return this._http.put<any>(this.getPath() + 'UpdateTest', item);
  }

  deleteTest(id: string): Observable<any> {
    return this._http.delete<any>(this.getPath() + 'DeleteTest/' + id);
  }

  editVariants(variants: Variant[]): Observable<any> {
    return this._http.put<any>(this.getPath() + 'UpdateVariants', variants);
  }

  getProjectExercises(topicId: string): Observable<ProjectExercise[]> {
    return this._http.get<ProjectExercise[]>(this.getPath() + 'ProjectExercises/' + topicId);
  }

  craeteProjectExercise(item: ProjectExercise): Observable<string> {
    return this._http.post<string>(this.getPath() + 'CreateProjectExercise', item);
  }

  editProjectExercise(item: ProjectExercise): Observable<any> {
    return this._http.put<any>(this.getPath() + 'UpdateProjectExercise', item);
  }

  deleteProjectExercise(id: string): Observable<any> {
    return this._http.delete<any>(this.getPath() + 'DeleteProjectExercise/' + id);
  }
}
