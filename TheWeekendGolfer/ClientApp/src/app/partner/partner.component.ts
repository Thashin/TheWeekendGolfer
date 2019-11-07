import { Component, ViewChild, OnInit } from "@angular/core";
import { PartnerService } from "../services/partner.service";
import { MatDialog } from "@angular/material/dialog";
import { MatPaginator } from "@angular/material/paginator";
import {
  SimpleSnackBar,
  MatSnackBarRef,
  MatSnackBar
} from "@angular/material/snack-bar";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { Player } from "../models/player.model";
import { UserService } from "../services/user.service";
import { AddPartnerDialogComponent } from "./add-partner-dialog.component";

@Component({
  templateUrl: "./partner.component.html",
  styleUrls: ["./partner.component.css"]
})
export class PartnerComponent implements OnInit {
  isLoggedIn = false;
  partners: MatTableDataSource<Player> | undefined;
  displayedColumns = ["name", "edit"];

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor(
    private _userService: UserService,
    private _partnerService: PartnerService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog
  ) {
    this.getPartners();
  }

  ngOnInit() {
    this.isLoggedIn = this._userService.getIsLoggedIn();
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(AddPartnerDialogComponent, {
      minWidth: "1000px"
    });

    dialogRef.afterClosed().subscribe(partner => {
      if (partner != null) {
        this._partnerService.addPartner(partner).subscribe(data => {
          if (data) {
            this.openSnackBar("Partner Created Successfully");
            this.getPartners();
          } else {
            this.openDialog();
            this.openSnackBar("Unable to create Partner");
          }
        });
      }
    });
  }

  openSnackBar(message: string): MatSnackBarRef<SimpleSnackBar> {
    return this.snackBar.open(message, "", {
      duration: 5000
    });
  }

  applyFilter(filterValue: string) {
    this.partners.filterPredicate = (data, filter) =>
      data.firstName
        .trim()
        .toLowerCase()
        .indexOf(filter) !== -1 ||
      data.lastName
        .trim()
        .toLowerCase()
        .indexOf(filter) !== -1;

    this.partners.filter = filterValue.trim().toLowerCase();

    if (this.partners.paginator) {
      this.partners.paginator.firstPage();
    }
  }

  getPartners() {
    this._userService.getPlayerid().subscribe(player => {
      this._partnerService.getPartners(player.id).subscribe(data => {
        this.partners = new MatTableDataSource(data);
        this.partners.paginator = this.paginator;
        this.partners.sortingDataAccessor = (partner, property) => {
          switch (property) {
            case "name":
              return partner.lastName.toLowerCase();
            default:
              return partner[property];
          }
        };
        this.partners.sort = this.sort;
      });
    });
  }

  removePartner(partnerId) {
    this._partnerService.removePartner(partnerId).subscribe(data => {
      if (data) {
        this.getPartners();
        this.openSnackBar("Partner Deleted Successfully");
      } else {
        this.openSnackBar("Unable to delete partner");
      }
    });
  }
}
