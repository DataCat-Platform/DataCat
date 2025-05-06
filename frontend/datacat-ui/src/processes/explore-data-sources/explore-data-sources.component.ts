import {Component} from '@angular/core';
import {Panel} from "primeng/panel";
import {CreateDataSourceButtonComponent} from "../../features/data-sources/create-data-source-button";

@Component({
    selector: 'app-explore-data-sources',
    standalone: true,
    imports: [
        Panel,
        CreateDataSourceButtonComponent
    ],
    templateUrl: './explore-data-sources.component.html',
    styleUrl: './explore-data-sources.component.scss'
})
export class ExploreDataSourcesComponent {

}
