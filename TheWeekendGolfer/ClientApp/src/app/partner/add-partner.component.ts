import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PartnerService } from "../services/partner.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { PlayerService } from '../services/player.service';
import { Player } from '../models/player.model';

@Component({
  selector: 'app-add-partner',
  templateUrl: './add-partner.component.html'
})
export class AddPartnerComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private router: Router, private partnerService: PartnerService, private _playerService: PlayerService) {
    this.getPlayers();
  }

  allPlayers:Player[]
  createPartnerForm: FormGroup;

  ngOnInit() {

    this.createPartnerForm = this.formBuilder.group({
      id: [],
      name: ['', Validators.required]
    });

  }

  getPlayers() {
    this._playerService.getPlayers().subscribe(data => this.allPlayers = data);
  }

  onSubmit() {
    this.partnerService.addPartner(this.createPartnerForm.value)
      .subscribe(data => {
        this.router.navigate(['partners']);
      });
  }

}
