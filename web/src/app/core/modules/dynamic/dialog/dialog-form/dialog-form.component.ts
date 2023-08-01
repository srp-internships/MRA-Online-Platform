import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Editor, Toolbar } from 'ngx-editor';
import { ControlBase } from 'src/app/core/form-elements';

@Component({
  selector: 'srp-dynamic-dialog-form',
  templateUrl: './dialog-form.component.html',
  styles: [':host {width: 100%} '],
})
export class DynamicDialogFormComponent implements OnInit, OnDestroy {
  ngOnInit(): void {
    this.editor.view.dom.style.maxHeight = '400px';
    this.editor.view.dom.style.overflow = 'auto';
  }

  maximized = false;
  @Input() control!: ControlBase;
  @Input() form!: FormGroup;
  get isValid() {
    const control = this.form.controls[this.control.key];
    return control.errors && control.touched;
  }

  editor: Editor = new Editor();
  toolbar: Toolbar = [
    ['bold', 'italic'],
    ['underline', 'strike'],
    ['code', 'blockquote'],
    ['ordered_list', 'bullet_list'],
    [{ heading: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
    ['link', 'image'],
    ['text_color', 'background_color'],
    ['align_left', 'align_center', 'align_right', 'align_justify'],
  ];

  editorOptions = {
    language: 'csharp',
    minimap: {
      enabled: false,
    },
    contextmenu: false,
    automaticLayout: true,
  };

  ngOnDestroy(): void {
    this.editor.destroy();
  }

  onClick(e: MouseEvent): void {
    e.preventDefault();
    this.editor.commands.insertNewLine().insertHTML('<hr>').insertNewLine().exec();
  }

  onClickCode(e: MouseEvent): void {
    e.preventDefault();
    this.editor.commands.insertNewLine().insertHTML('<pre >Code...</pre>').exec();
  }

  onToggleSceen(e: MouseEvent): void {
    e.preventDefault();
    this.maximized = !this.maximized;
    if (this.maximized) {
      this.editor.view.dom.style.maxHeight = 'calc(100vh - 50px)';
    } else {
      this.editor.view.dom.style.maxHeight = '400px';
    }
  }
}
