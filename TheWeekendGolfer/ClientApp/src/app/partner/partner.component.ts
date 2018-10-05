import { Component, Inject, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { PartnerService } from '../services/partner.service'
import { Partner } from '../models/partner.model'
import { MatTableDataSource, MatPaginator, MatSort, SimpleSnackBar, MatSnackBarRef, MatSnackBar, MatDialog } from '@angular/material';
import { Player } from '../models/player.model';
import { UserService } from '../services/user.service';
import { AddPartnerDialogComponent } from './add-partner-dialog.component';

@Component({
  templateUrl: './partner.component.html',
  styleUrls: ['./partner.component.css']
})

export class PartnerComponent implements OnInit {


  isLoggedIn: boolean = false;
  public partners: MatTableDataSource<Player> | null;
  displayedColumns: string[] = ['name', 'edit'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private _userService: UserService, private _partnerService: PartnerService, private snackBar: MatSnackBar, public dialog: MatDialog) {
    this.getPartners();
  }

  ngOnInit() {
    this.isLoggedIn = this._userService.getIsLoggedIn();
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(AddPartnerDialogComponent, {
      minWidth: '1000px',
    });

    dialogRef.afterClosed().subscribe(partner => {
      console.log(partner)
      if (partner != null) {
        this._partnerService.addPartner(partner).subscribe(data => {
          if (data) {
            this.openSnackBar("Partner Created Successfully");
            this.getPartners();
          }
          else {
            this.openDialog();
            this.openSnackBar("Unable to create Partner");
          }
        });
      }
    });
  }

  openSnackBar(message: string): MatSnackBarRef<SimpleSnackBar> {
    return this.snackBar.open(message, "", {
      duration: 5000,
    });
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
    this._partnerService.removePartner(partnerId).subscribe(data => {
      if (data) {
        this.getPartners();
        this.openSnackBar("Partner Deleted Successfully");
      }
      else {
        this.openSnackBar("Unable to delete partner");
      }
    });
  }
}
