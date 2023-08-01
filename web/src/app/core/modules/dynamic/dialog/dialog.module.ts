import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CodemirrorModule } from '@ctrl/ngx-codemirror';
import { NgxEditorModule } from 'ngx-editor';
import { DynamicDialogFormComponent } from './dialog-form/dialog-form.component';
import { DynamicDialogComponent } from './dialog.component';

@NgModule({
  declarations: [DynamicDialogComponent, DynamicDialogFormComponent],
  exports: [DynamicDialogComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgxEditorModule, CodemirrorModule],
})
export class DynamicDialogModule {}
