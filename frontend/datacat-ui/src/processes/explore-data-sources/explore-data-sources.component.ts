import {Component} from '@angular/core';
import {Panel} from "primeng/panel";
import {CreateDataSourceButtonComponent} from "../../features/data-sources/create-data-source-button";
import {
    DataSourcesFilteringComponent
} from "../../features/data-sources/data-sources-filtering/data-sources-filtering.component";
import {DataSourcesFilter} from "../../entities/data-sources/data-sources-filter";
import {DataSourcesListComponent} from "../../features/data-sources/data-sources-list/data-sources-list.component";

@Component({
    selector: 'app-explore-data-sources',
    standalone: true,
    imports: [
        Panel,
        CreateDataSourceButtonComponent,
        DataSourcesFilteringComponent,
        DataSourcesListComponent
    ],
    templateUrl: './explore-data-sources.component.html',
    styleUrl: './explore-data-sources.component.scss'
})
export class ExploreDataSourcesComponent {
    filter: DataSourcesFilter | undefined = undefined;

    onFilterChange(filter: DataSourcesFilter) {
        this.filter = filter;
        console.log('New filter:', filter);
    }
}
