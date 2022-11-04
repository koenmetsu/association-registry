﻿namespace AssociationRegistry.Test.Admin.Api.UnitTests.Validators;

using AssociationRegistry.Admin.Api.Verenigingen;
using FluentValidation.TestHelper;
using Framework;
using Xunit;

public class When_Validating_A_RegistreerVerenigingRequest
{
    public class Given_Name_Is_Empty_String : ValidatorTest
    {
        [Fact]
        public void Then_it_has_validation_error__naam_mag_niet_leeg_zijn()
        {
            var validator = new RegistreerVerenigingRequestValidator();
            var result = validator.TestValidate(new RegistreerVerenigingRequest { Naam = "" });

            result.ShouldHaveValidationErrorFor(vereniging => vereniging.Naam)
                .WithErrorMessage("'Naam' mag niet leeg zijn.")
                .Only();
        }
    }

    public class Given_Name_Is_Null : ValidatorTest
    {
        [Fact]
        public void Then_it_has_validation_error__naam_is_verplicht()
        {
            var validator = new RegistreerVerenigingRequestValidator();
            var result = validator.TestValidate(new RegistreerVerenigingRequest());

            result.ShouldHaveValidationErrorFor(vereniging => vereniging.Naam)
                .WithErrorMessage("'Naam' is verplicht.");
        }
    }

    public class Given_A_Valid_Naam : ValidatorTest
    {
        [Fact]
        public void Then_it_has_no_validation_errors()
        {
            var validator = new RegistreerVerenigingRequestValidator();
            var result = validator.TestValidate(new RegistreerVerenigingRequest { Naam = "abcd" });

            result.ShouldNotHaveValidationErrorFor(vereniging => vereniging.Naam);
        }
    }

    public class Given_A_Valid_Request : ValidatorTest
    {
        [Fact]
        public void Then_it_has_no_validation_errors()
        {
            var validator = new RegistreerVerenigingRequestValidator();
            var result = validator.TestValidate(new RegistreerVerenigingRequest { Naam = "abcd" });

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}