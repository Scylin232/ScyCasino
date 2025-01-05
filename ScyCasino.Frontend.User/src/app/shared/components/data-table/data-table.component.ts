import {Component, inject, Input, OnInit} from '@angular/core';
import {NgForOf} from '@angular/common';
import {HttpClient} from '@angular/common/http';

import {PaginatedResponse} from '../../models/api.model';
import {DataTableEntry, DataTableEntryMapping} from '../../models/data-table.model';

import {environment} from '../../../../environments/environment';
import {PaginationComponent} from '../pagination/pagination.component';

@Component({
  selector: 'app-data-table',
  imports: [
    NgForOf,
    PaginationComponent
  ],
  templateUrl: './data-table.component.html',
  styleUrl: './data-table.component.css'
})
export class DataTableComponent<T extends DataTableEntry> implements OnInit {
  @Input() title!: string;
  @Input() description!: string;
  @Input() path!: string;
  @Input() entryMappings!: DataTableEntryMapping[];

  private readonly http: HttpClient = inject(HttpClient);

  public totalPages: number = 0;
  public totalCount: number = 0;
  public currentPage: number = 1;
  public currentCount: number = 15;

  public entries: T[] = [];

  public ngOnInit(): void {
    this.fetchTableData();
  }

  public onPageChange(page: number): void {
    this.currentPage = page;
    this.fetchTableData();
  }

  public onCountChange(count: number): void {
    const newTotalPages: number = Math.ceil(this.totalCount / count);

    if (this.currentPage > newTotalPages)
      this.currentPage = newTotalPages;

    this.currentCount = count;
    this.fetchTableData();
  }

  public fetchTableData(): void {
    this.http.get<PaginatedResponse<T>>(`${environment.apiUrl}/${this.path}?page=${this.currentPage}&count=${this.currentCount}`).subscribe({
      next: (data: PaginatedResponse<T>): void => {
        this.totalPages = data.totalPages;
        this.totalCount = data.totalCount;

        this.entries = data.results;
      },
    })
  }
}
