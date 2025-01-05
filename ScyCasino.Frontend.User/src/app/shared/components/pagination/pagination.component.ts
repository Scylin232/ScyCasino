import {Component, EventEmitter, Input, OnChanges, Output} from '@angular/core';
import {NgForOf} from '@angular/common';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-pagination',
  imports: [
    NgForOf,
    FormsModule
  ],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent implements OnChanges {
  @Input() totalPages!: number;
  @Input() totalCount!: number;
  @Input() currentPage!: number;
  @Input() currentCount!: number;
  @Input() pageSkip: number = 2;

  @Output() pageChange: EventEmitter<number> = new EventEmitter<number>();
  @Output() countChange: EventEmitter<number> = new EventEmitter<number>();

  public availablePages: number[] = [];

  public ngOnChanges(): void {
    this.availablePages = this.getPaginationNumbers(this.totalPages, this.currentPage);
  }

  public selectCount(): void {
    if (this.currentCount < 0) return;

    this.countChange.emit(this.currentCount);
  }

  public selectPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;

    this.pageChange.emit(page);
  }

  private getPaginationNumbers(totalPages: number, currentPage: number): number[] {
    const pageNumbers: number[] = Array.from({ length: totalPages }, (_: unknown, index: number): number => index + 1);

    return pageNumbers.filter(
      (pageNumber: number): boolean =>
        pageNumber === 1 ||
        pageNumber === totalPages ||
        (pageNumber >= currentPage - this.pageSkip && pageNumber <= currentPage + this.pageSkip)
    );
  };
}
