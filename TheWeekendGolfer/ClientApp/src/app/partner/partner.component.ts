import { Component, Inject, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { PartnerService } from '../services/partner.service'
import { Partner } from '../models/partner.model'
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { Player } from '../models/player.model';
import { UserService } from '../services/user.service';

@Component({
  templateUrl: './partner.component.html',
  styleUrls:['./partner.component.css']
})

export class PartnerComponent{


  public partners: MatTableDataSource<Player> | null;
  displayedColumns: string[] = ['name','edit'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private _userService: UserService, private _partnerService: PartnerService) {
    this.getPartners();
  }


  applyFilter(filterValue: string) {
    this.partners.filterPredicate = (data, filter) =>
      (
        data.firstName.trim().toLowerCase().indexOf(filter) !== -1 ||
        data.lastName.trim().toLowerCase().indexOf(filter) !== -1
      );


    this.partners.filter = filterValue.trim().toLowerCase();

    if (this.partners.paginator) {
      this.partners.paginator.firstPage();
    }
  }

  getPartners() {
    this._userService.getPlayerid().subscribe(
      player => {
        this._partnerService.getPartners(player.id).subscribe(
          data => {
            this.partners = new MatTableDataSource(data);
            this.partners.paginator = this.paginator;
            this.partners.sortingDataAccessor = (partner, property) => {
              switch (property) {
                case 'name': return partner.lastName.toLowerCase();
                default: return partner[property];
              }
            };
            this.partners.sort = this.sort;
          }
        );
      })
  }

  removePartner(partnerId) {
    console.log(partnerId);
    this._partnerService.removePartner(partnerId).subscribe(data => this.getPartners());
  }
}
