import { Component, OnInit } from "@angular/core";
import { Validators, FormControl } from "@angular/forms";
import { PartnerService } from "../services/partner.service";
import { Player } from "../models/player.model";
import { UserService } from "../services/user.service";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
  templateUrl: "./add-partner-dialog.component.html"
})
export class AddPartnerDialogComponent implements OnInit {
  allPlayers: Player[];

  partner = new FormControl("", [Validators.required]);

  constructor(
    private _partnerService: PartnerService,
    private _userService: UserService,
    public dialog: MatDialogRef<AddPartnerDialogComponent>
  ) {}

  ngOnInit() {
    this.getPlayers();
  }

  getPlayers() {
    this._userService.getPlayerid().subscribe(player => {
      this._partnerService
        .getPotentialPartners(player.id)
        .subscribe(potentialPartners => {
          this.allPlayers = potentialPartners;
        });
    });
  }
  onNoClick(): void {
    this.dialog.close();
  }

  submitPartner() {
    this.dialog.close(this.partner.value);
  }
}
