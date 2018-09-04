import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from "@angular/forms";
import { PartnerService } from "../services/partner.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { PlayerService } from '../services/player.service';
import { Player } from '../models/player.model';
import { UserService } from '../services/user.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';

@Component({
  templateUrl: './../home/home.component.html'
})
export class AddPartnerComponent implements OnInit {

  allPlayers: Player[]

  createPartnerForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private router: Router, private partnerService: PartnerService, private _userService: UserService, public dialog: MatDialog) {
    this.createPartnerForm = this.formBuilder.group({
      id: [],
      partnerId: ['', Validators.required]
    });
    this.openDialog();
  }

  ngOnInit() {

  }

  openDialog(): void {
    
    const dialogRef = this.dialog.open(AddPartnerDialog, {
      minWidth: '500px',
      data: this.createPartnerForm
    });

    dialogRef.afterClosed().subscribe(result => {
      this.partnerService.addPartner(result.value.partnerId)
        .subscribe(data => {
          this.router.navigate(['/']);
        });
    });
  }

  onSubmit() {
    this.partnerService.addPartner(this.createPartnerForm.value)
      .subscribe(data => {
        this.router.navigate(['/']);
      });
  }

}
@Component({
  templateUrl: './add-partner.component.html',
})
export class AddPartnerDialog{
  allPlayers: Player[]

  constructor(private _partnerService: PartnerService,  private _userService: UserService, private formBuilder: FormBuilder,
    public dialog: MatDialogRef<AddPartnerDialog>, @Inject(MAT_DIALOG_DATA) public data: FormArray) {
    this.getPlayers();
  }


  getPlayers() {
    this._userService.getPlayerid().subscribe(playerId => {
      this._partnerService.getPotentialPartners(playerId).subscribe(potentialPartners => {
        this.allPlayers = potentialPartners;
        console.log(this.allPlayers);
      });
    });
      }

  onNoClick(): void {
    this.dialog.close();
  }
}
