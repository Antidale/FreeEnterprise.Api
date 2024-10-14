using System;
using System.Net;
using FluentAssertions;
using FreeEnterprise.Api.Classes;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.UnitTests.ClassesTests;

public class ResponseTests
{
    [Test]
    public void Response_SetSuccess_SetsSuccess_True()
    {
        var expectedData = "success string";
        var sut = new Response<string>();

        sut.SetSuccess(expectedData);

        Assert.Multiple(() =>
        {
            sut.Success.Should().BeTrue("After setting success, the Success property should be true");
            sut.Data.Should().Be(expectedData, "The object passed in to SetSuccess should set the Data property");
            sut.ErrorStatusCode.Should().BeNull("successful requests should not have an error status code");
            sut.ErrorMessage.Should().BeNullOrWhiteSpace("successful requests should have no error message");
        });
    }

    [Test]
    public void Response_Success_ShouldStart_False()
    {
        var sut = new Response<string>();
        sut.Success.Should().BeFalse("The Success property should default to false");
    }

    [Test]
    public void Response_BadRequest_ShouldSet_StatusCode_and_Message()
    {
        var errorMessage = "Oh, caught in a Bad Request";
        var sut = new Response<int>();

        sut.BadRequest(errorMessage);

        sut.ErrorStatusCode.Should().Be(HttpStatusCode.BadRequest);
        sut.Data.Should().Be(default, "an errored response should have a default value for T");
    }

    [Test]
    public void Response_Unauthorized_ShouldSet_StatusCode_and_Message()
    {
        var errorMessage = "Don't go in there";
        var sut = new Response<int>();

        sut.Unauthorized(errorMessage);

        sut.ErrorStatusCode.Should().Be(HttpStatusCode.Unauthorized);
        sut.Data.Should().Be(default, "an errored response should have a default value for T");
    }

    [Test]
    public void Response_NotFound_ShouldSet_StatusCode_and_Message()
    {
        var errorMessage = "404";
        var sut = new Response<int>();

        sut.NotFound(errorMessage);

        sut.ErrorStatusCode.Should().Be(HttpStatusCode.NotFound);
        sut.Data.Should().Be(default, "an errored response should have a default value for T");
    }

    [Test]
    public void Response_Conflict_ShouldSet_StatusCode_and_Message()
    {
        var errorMessage = "Fite!";
        var sut = new Response<int>();

        sut.Conflict(errorMessage);

        sut.ErrorStatusCode.Should().Be(HttpStatusCode.Conflict);
        sut.Data.Should().Be(default, "an errored response should have a default value for T");
    }

    [Test]
    public void Response_InternalServerError_ShouldSet_StatusCode_and_Message()
    {
        var errorMessage = "Dave, I can't do that";
        var sut = new Response<int>();

        sut.InternalServerError(errorMessage);

        sut.ErrorStatusCode.Should().Be(HttpStatusCode.InternalServerError);
        sut.Data.Should().Be(default, "an errored response should have a default value for T");
    }

    [Test]
    public void GetRequestResponse_Success_Should_Return_OkObject()
    {
        var sut = new Response<int>();
        sut.SetSuccess(42);

        var response = sut.GetRequestResponse();
        response.GetType().Should().Be(typeof(OkObjectResult));
        response.Value.Should().Be(42);
    }

    [Test]
    public void GetRequestResponse_BadRequest_ShouldReturn_BadRequestObject()
    {
        var errorMessage = "Oh, caught in a Bad Request";
        var sut = new Response<int>();

        sut.BadRequest(errorMessage);

        var response = sut.GetRequestResponse();
        response.GetType().Should().Be(typeof(BadRequestObjectResult));
        response.Value.Should().Be(errorMessage);
    }

    [Test]
    public void GetRequestResponse_Unauthorized_ShouldReturn_UnauthorizedRequestObject()
    {
        var errorMessage = "I can't allow that, Dave";
        var sut = new Response<int>();

        sut.Unauthorized(errorMessage);

        var response = sut.GetRequestResponse();
        response.GetType().Should().Be(typeof(UnauthorizedObjectResult));
        response.Value.Should().Be(errorMessage);
    }

    [Test]
    public void GetRequestResponse_NotFound_ShouldReturn_NotFoundRequestObject()
    {
        var errorMessage = "404";
        var sut = new Response<int>();

        sut.NotFound(errorMessage);

        var response = sut.GetRequestResponse();
        response.GetType().Should().Be(typeof(NotFoundObjectResult));
        response.Value.Should().Be("404");
    }

    [Test]
    public void GetRequestResponse_Conflict_ShouldReturn_ConflictRequestObject()
    {
        var errorMessage = "Fite!";
        var sut = new Response<int>();

        sut.Conflict(errorMessage);

        var response = sut.GetRequestResponse();
        response.GetType().Should().Be(typeof(ConflictObjectResult));
        response.Value.Should().Be("Fite!");
    }

    [Test]
    public void GetRequestResponse_InternalServerError_ShouldThrow()
    {
        var errorMessage = "Dave, I can't do that";

        var sut = new Response<int>();
        sut.InternalServerError(errorMessage);

        Assert.Throws<InvalidOperationException>(() => sut.GetRequestResponse());
    }
}
