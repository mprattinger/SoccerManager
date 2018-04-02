import { Injectable } from '@angular/core';

@Injectable()
export class ToolsService {

  constructor() { }

  createGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
}

//d3312c0d-a545-4ea9-9a43-fad7dba72f28
//xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx